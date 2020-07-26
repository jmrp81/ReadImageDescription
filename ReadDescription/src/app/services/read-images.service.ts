import { Injectable, OnInit } from '@angular/core';
import * as piexif from 'piexifjs';
import { GaleryImage } from '../models/galery-image';

@Injectable({
  providedIn: 'root'
})
export class ReadImagesService {
  imageList = new Array<GaleryImage>();
  imagesNumber = 3;

  constructor() { }

  getImageInformation(): Array<GaleryImage> {
    for (let i = 1; i <= this.imagesNumber; i++) {
      let currentImage = new GaleryImage();
      currentImage.imageName = 'Image (' + i + ').jpg';
      currentImage.url = '../../assets/images/' + currentImage.imageName;
      currentImage.alt = 'Imagen sin descripción.';
      currentImage.autor = 'ReadImaeDescription';

      currentImage = this.getBase64Image(currentImage);
      this.imageList.push(currentImage);
    }

    return this.imageList;
  }

  getBase64Image(currentImage: GaleryImage): GaleryImage {
    const xhr = new XMLHttpRequest();

    xhr.onload = () => {
      const reader = new FileReader();
      reader.onloadend = () => {
        currentImage.imageBase64 = reader.result;
        const description = this.loadedImageDescription(null, currentImage);
        currentImage.alt = description;
      };
      currentImage.imageBytes = xhr.response;
      reader.readAsDataURL(xhr.response);
    };

    xhr.open('GET', currentImage.url);
    xhr.responseType = 'blob';
    xhr.send();

    return currentImage;
  }

  loadedImageDescription(event: any, currentImage: GaleryImage): any {
    const infoObtained = piexif.load(currentImage.imageBase64);
    const zeroTh = infoObtained['0th'];
    return zeroTh[270];
  }
}
