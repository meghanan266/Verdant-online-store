export class OrderResponse {
    public id: string;
    public entity: string;
    public amount: number;
    public amount_paid: number;
    public amount_due: number;
    public currency: "INR";
    public receipt: string;
    public status: string;
    public attempts: number;
    public notes: [];
    public created_at: Date;
    public error: {
        code: string;
        description: string;
        source: string;
        step: string;
        reason: string;
        metadata: {};
        field: string;
    }
}