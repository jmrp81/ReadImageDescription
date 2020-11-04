import { Injectable, OnInit } from '@angular/core';
import * as piexif from 'piexifjs';
import * as saveAs from 'file-saver';
import {Buffer} from 'buffer';

import { GaleryImage } from '../models/galery-image';

@Injectable({
  providedIn: 'root'
})
export class ReadImagesService {
  imageList = new Array<GaleryImage>();
  imagesNumber = 2;

  constructor() { }

  getImageInformation(): Array<GaleryImage> {
    for (let i = 1; i <= this.imagesNumber; i++) {
      const currentImage = new GaleryImage();
      currentImage.imageName = 'Image (' + i + ').jpg';
      currentImage.url = '../../assets/images/' + currentImage.imageName;
      currentImage.alt = 'Imagen sin descripción.';
      currentImage.autor = 'ReadImaeDescription';

      this.imageList.push(currentImage);
    }

    return this.imageList;
  }

  loadedImageDescription(event: any, currentImage: GaleryImage): any {
    const infoObtained = piexif.load(currentImage.imageBase64);
    const zeroTh = infoObtained['0th'];
    return zeroTh[270];
  }

  modifyDescription(event: any, currentImage: GaleryImage): void {
    const infoObtained = piexif.load(currentImage.imageBase64);
    const zeroTh = infoObtained['0th'];
    zeroTh[270] = currentImage.description;
    infoObtained.Exif[37510] = currentImage.description;
    const exifStr = piexif.dump(infoObtained); // Get exif as string to insert into JPEG.
    const insertedInfoToJPEG = piexif.insert(exifStr, currentImage.imageBase64);

    const newJpeg = Buffer.from(insertedInfoToJPEG, 'binary');
    saveAs(insertedInfoToJPEG, currentImage.imageName);
  }

  getBase64Image(imageSRC: any, currentImage: GaleryImage): void {
    const xhr = new XMLHttpRequest();

    xhr.onload = () => {
      const reader = new FileReader();
      reader.onloadend = () => {
        currentImage.imageBase64 = reader.result;
      };
      currentImage.imageBytes = xhr.response;
      reader.readAsDataURL(xhr.response);
    };

    xhr.open('GET', imageSRC);
    xhr.responseType = 'blob';
    xhr.send();
  }
}
