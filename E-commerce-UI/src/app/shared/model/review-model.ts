export class Review {
    public reviewId: number;
    public productId: number;
    public reviewDescription: string = "";
    public topReview: boolean;
    public reviewDate: Date;
    public userName: string;
    public reviewRange: number;
}