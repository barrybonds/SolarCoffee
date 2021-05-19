export interface IInventoryTimeline {
   productInventorySnapshots: ISnapshot[];
   timeline: Date[];
}

export interface ISnapshot {
    productid:number;
    quantityOnHand:number[];
}