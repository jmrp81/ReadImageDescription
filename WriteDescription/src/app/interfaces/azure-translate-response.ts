import { AzureTranslateDetectedLanguage } from './azure-translate-detected-language';
import { AzureTranslateTranslation } from './azure-translate-translation';

export interface AzureTranslateResponse {
    detectedLanguage: AzureTranslateDetectedLanguage;
    translations: Array<AzureTranslateTranslation>;
}
