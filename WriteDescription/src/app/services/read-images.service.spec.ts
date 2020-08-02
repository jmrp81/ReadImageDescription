import { TestBed } from '@angular/core/testing';

import { ReadImagesService } from './read-images.service';

describe('ReadImagesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReadImagesService = TestBed.get(ReadImagesService);
    expect(service).toBeTruthy();
  });
});
