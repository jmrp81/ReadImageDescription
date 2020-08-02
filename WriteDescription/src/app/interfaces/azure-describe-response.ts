import { AzureMetada } from './azure-metada';
import { AzureDescription } from './azure-description';

export interface AzureDescribeResponse {
    description: AzureDescription;
    requestId: string;
    metadata: AzureMetada;
}
