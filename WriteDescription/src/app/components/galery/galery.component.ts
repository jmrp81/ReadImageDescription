import { Component, OnInit } from '@angular/core';
import { GaleryImage } from 'src/app/models/galery-image';
import { AzureService } from 'src/app/services/azure.service';
import { ReadImagesService } from 'src/app/services/read-images.service';

@Component({
  selector: 'app-galery',
  templateUrl: './galery.component.html',
  styleUrls: ['./galery.component.scss']
})
export class GaleryComponent implements OnInit {
  imageList = new Array<GaleryImage>();
  errorDescription: any;

  constructor(private readImagesService: ReadImagesService, private azureService: AzureService) {
  }

  ngOnInit() {
    this.getImageInformation();
  }

  getImageInfo(event: any, actualImage: GaleryImage): void {
    this.readImagesService.getBase64Image(event.target.src, actualImage);
  }

  getImagaDescription(event: any, actualImage: GaleryImage): void {
    this.readImagesService.loadedImageDescription(event, actualImage);
  }

  getImageInformation(): void {
    this.imageList = this.readImagesService.getImageInformation();
  }

  setImageDescription(event: any, actualImage: GaleryImage): void {
    this.azureService.getDescriptionImage(actualImage.imageBytes).subscribe(azureDescribeResponse => {
      actualImage.description = 'Imagen sin descripción';
      if (typeof (azureDescribeResponse.description.captions[0]) !== 'undefined'
        && azureDescribeResponse.description.captions[0] != null
        && azureDescribeResponse.description.captions.length > 0) {
        actualImage.description = azureDescribeResponse.description.captions[0].text;
      } else if (typeof (azureDescribeResponse.description.tags) !== 'undefined'
        && azureDescribeResponse.description.tags != null) {
        actualImage.description = 'Imagen que contiene: ';
        const value = azureDescribeResponse.description.tags.length > 3 ? 3 : azureDescribeResponse.description.tags.length;
        for (let i = 0; i < value; i++) {
          actualImage.description = actualImage.description + azureDescribeResponse.description.tags[i] + ', ';
        }

        actualImage.description = actualImage.description.substring(0, actualImage.description.length - 2);
      }
      actualImage.alt = actualImage.description;
      this.readImagesService.modifyDescription(event, actualImage);
    },
      error => this.errorDescription = error);
  }
}
