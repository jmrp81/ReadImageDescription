import { Component, OnInit } from '@angular/core';
import { GaleryImage } from 'src/app/models/galery-image';
import { AzureService } from 'src/app/services/azure.service';
import { ReadImagesService } from 'src/app/services/read-images.service';
import { AzureTranslateRequest } from 'src/app/interfaces/azure-translate-request';

@Component({
  selector: 'app-galery',
  templateUrl: './galery.component.html',
  styleUrls: ['./galery.component.scss']
})
export class GaleryComponent implements OnInit {
  imageList = new Array<GaleryImage>();
  errorDescription: any;
  isCheckToTranslate = false;

  constructor(private readImagesService: ReadImagesService, private azureService: AzureService) {
  }

  ngOnInit() {
    this.getImageInformation();
  }

  getImageInfo(event: any, currentImage: GaleryImage): void {
    this.readImagesService.getBase64Image(event.target.src, currentImage);
  }

  getImagaDescription(event: any, currentImage: GaleryImage): void {
    this.readImagesService.loadedImageDescription(event, currentImage);
  }

  getImageInformation(): void {
    this.imageList = this.readImagesService.getImageInformation();
  }

  setImageDescription(event: any, currentImage: GaleryImage): void {
    this.azureService.getDescriptionImage(currentImage.imageBytes)
    .subscribe(azureDescribeResponse => {

      currentImage.description = 'Imagen sin descripción';

      if (typeof (azureDescribeResponse.description.captions[0]) !== 'undefined'
        && azureDescribeResponse.description.captions[0] != null
        && azureDescribeResponse.description.captions.length > 0) {

        currentImage.description = azureDescribeResponse.description.captions[0].text;

      } else if (typeof (azureDescribeResponse.description.tags) !== 'undefined'
                 && azureDescribeResponse.description.tags != null) {

        currentImage.description = 'Imagen que contiene: ';
        const value = azureDescribeResponse.description.tags.length > 3 ? 3 : azureDescribeResponse.description.tags.length;

        for (let i = 0; i < value; i++) {
          currentImage.description = currentImage.description + azureDescribeResponse.description.tags[i] + ', ';
        }

        currentImage.description = currentImage.description.substring(0, currentImage.description.length - 2);
      }

      currentImage.alt = currentImage.description;

      if (this.isCheckToTranslate) {
        const azureTranslateRequest = { Text: currentImage.description};
        const azureTranslateRequestList = new Array<AzureTranslateRequest>();

        azureTranslateRequestList.push(azureTranslateRequest);

        this.azureService.getTranslateDescriptionImage(azureTranslateRequestList)
        .subscribe(azureTranslateResponse => {
          if (azureTranslateResponse[0] != null
              && typeof (azureTranslateResponse[0]) !== 'undefined') {

              const firstDescriptionObj = azureTranslateResponse[0].translations[0];

              if (firstDescriptionObj != null && typeof (firstDescriptionObj) !== 'undefined') {
                currentImage.alt = firstDescriptionObj.text;
                currentImage.description = firstDescriptionObj.text;
              }
          }

          this.readImagesService.modifyDescription(event, currentImage);
        },
        error => {
          this.readImagesService.modifyDescription(event, currentImage);
          this.errorDescription = error;
        });
      } else {
        this.readImagesService.modifyDescription(event, currentImage);
      }
    },
      error => this.errorDescription = error);
  }
}
