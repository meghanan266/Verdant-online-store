export class Product {
    productId: number;
    productName: string;
    price: number;
    pictureUrl: string[];
    productDescription: string;
    productQuantity: string;
    rowStatus?: RowStatus;
}

export enum RowStatus {
    'NEW' = 1,
    'EDITED' = 2,
    'DELETED' = 3
}