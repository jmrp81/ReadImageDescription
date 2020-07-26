import { Component, OnInit } from '@angular/core';
import { GaleryImage } from 'src/app/models/galery-image';
import { ReadImagesService } from 'src/app/services/read-images.service';

@Component({
  selector: 'app-galery',
  templateUrl: './galery.component.html',
  styleUrls: ['./galery.component.scss']
})
export class GaleryComponent implements OnInit {
  imageList = new Array<GaleryImage>();
  errorDescription: any;

  constructor(private readImagesService: ReadImagesService) {
  }

  ngOnInit() {
    this.getImageInformation();
  }
  
  getImageInformation(): void {
    this.imageList = this.readImagesService.getImageInformation();
  }
}
