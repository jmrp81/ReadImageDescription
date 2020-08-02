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
  imagesNumber = 1;

  constructor() { }

  getImageInformation(): Array<GaleryImage> {
    for (let i = 1; i <= this.imagesNumber; i++) {
      const actualImage = new GaleryImage();
      actualImage.imageName = 'Image (' + i + ').jpg';
      actualImage.url = '../../assets/images/' + actualImage.imageName;
      actualImage.alt = 'Imagen sin descripción.';
      actualImage.autor = 'ReadImaeDescription';

      this.imageList.push(actualImage);
    }

    return this.imageList;
  }

  loadedImageDescription(event: any, actualImage: GaleryImage): any {
    const infoObtained = piexif.load(actualImage.imageBase64);
    const zeroTh = infoObtained['0th'];
    return zeroTh[270];
  }

  // https://github.com/hMatoba/piexifjs
  modifyDescription(event: any, actualImage: GaleryImage): void {
    const infoObtained = piexif.load(actualImage.imageBase64);
    const zeroTh = infoObtained['0th'];
    zeroTh[270] = actualImage.description;
    infoObtained.Exif[37510] = actualImage.description;
    const exifStr = piexif.dump(infoObtained); // Get exif as string to insert into JPEG.
    const insertedInfoToJPEG = piexif.insert(exifStr, actualImage.imageBase64);
    // piexif.remove(actualImage.imageData); // Remove exif from JPEG.

    const newJpeg = Buffer.from(insertedInfoToJPEG, 'binary'); // install npm i @types/node type definitions node
    saveAs(insertedInfoToJPEG, actualImage.imageName);
  }

  getBase64Image(imageSRC: any, actualImage: GaleryImage): void {
    const xhr = new XMLHttpRequest();

    xhr.onload = () => {
      const reader = new FileReader();
      reader.onloadend = () => {
        actualImage.imageBase64 = reader.result;
      };
      actualImage.imageBytes = xhr.response;
      reader.readAsDataURL(xhr.response);
    };

    xhr.open('GET', imageSRC);
    xhr.responseType = 'blob';
    xhr.send();
  }
}
