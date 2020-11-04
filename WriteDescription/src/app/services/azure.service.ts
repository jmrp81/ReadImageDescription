import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AzureDescribeResponse } from '../interfaces/azure-describe-response';
import { AzureTranslateRequest } from '../interfaces/azure-translate-request';
import { AzureTranslateResponse } from '../interfaces/azure-translate-response';

@Injectable({
  providedIn: 'root'
})
export class AzureService {
  // info API --> https://westus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fe

  // Cognitive services
  keyAzure = '';
  urlAzure = '';
  maxCandidate = 1; // valor por defecto 1, max 3
  language = 'en'; // en, ja, pt, zh, es
  urlDescribe = this.urlAzure + 'vision/v2.0/describe?maxCandidates=' + this.maxCandidate + '&language=' + this.language;
  // maxCandidates and language are optionals

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/octet-stream',
      'Ocp-Apim-Subscription-Key': this.keyAzure
    })
  };

  // Translate services
  azureTranslateKey = '';
  azureGeneralTranslateURL = 'https://api-eur.cognitive.microsofttranslator.com/translate';
  urlTranslate = this.azureGeneralTranslateURL + '?api-version=3.0&to=es';
  azureTranslateRegion = 'westeurope';
  useAzureTranslate = 'false';

  httpOptionsTranslate = {
    headers: new HttpHeaders({
      'Ocp-Apim-Subscription-Key': this.azureTranslateKey,
      'Ocp-Apim-Subscription-Region': this.azureTranslateRegion
    })
  };

  constructor(private http: HttpClient) { }

  getDescriptionImage(image: string): Observable<AzureDescribeResponse> {
    return this.http.post<AzureDescribeResponse>(this.urlDescribe, image, this.httpOptions);
  }

  getTranslateDescriptionImage(azureTranslateRequest: Array<AzureTranslateRequest>): Observable<Array<AzureTranslateResponse>> {
    return this.http.post<Array<AzureTranslateResponse>>(this.urlTranslate, azureTranslateRequest, this.httpOptionsTranslate);
  }
}
