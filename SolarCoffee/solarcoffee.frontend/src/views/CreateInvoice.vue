<template>
<div>
  <h1 id="invoiceTitle">
    Create Invoice
  </h1>
  <hr/>
  <div class="invoice-step" v-if="invoiceStep === 1">
    <h2>Step 1: Select Customer</h2>
    <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select v-model="selectedCustomerId" class="invoice-customers" id="customer">
          <option disabled value="">Please select a customer</option>
          <option v-for="c in customers" :value="c.id">
            {{ c.firstName + " " + c.lastName }}
          </option>
        </select>
    </div>
    
  <div class="invoice-steps-actions">
    <solar-button @button:click="prev" :disabled="!canGoPrev">Previous</solar-button>
    <solar-button @button:click="next" :disabled="!canGoNext">Next</solar-button>
    <solar-button @button:click="startOver">Start Over</solar-button>
  </div>
  </div>

  <div class="invoice-step" v-if="invoiceStep === 2">
    <h2>Step 2: Create Order</h2>
    <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select v-model="newItem.product" class="invoiceLineItem" id="product">
          <option disabled value="">Please select a Product</option>
          <option v-for="i in inventory" :value="i.product" :key="i.product.id">
            {{ i.product.name }}
          </option>
        </select>
        <label for="quantity">Quantity:</label>
        <input v-model="newItem.quantity" id="quantity" type="number" min="0">
    </div>
      
      <div class="invoice-item-actions">
    <solar-button @button:click="addLineItem" :disabled="!newItem.product || !newItem.quantity">Add Line Item</solar-button>
    <solar-button @button:click="finalizeOrder" :disabled="!lineItems.length">Finalize Order</solar-button>
  </div>
  </div>

  <div class="invoice-step" v-if="invoiceStep === 3">
    
  </div>
  <hr/>


</div>

</template> 

<script lang="ts">
import {Component, Vue} from "vue-property-decorator";
import {IInvoice, ILineItem} from "@/types/Invoice";
import {ICustomer} from "@/types/Customer";
import {IProductInventory} from "@/types/Product";
import CustomerService from "@/services/customer-service";
import {InventoryService} from "@/services/inventory-service";
import InvoiceService from "@/services/invoice-service";
import SolarButton from "@/components/SolarButton.vue";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
  name: "Create Invoice", components:{SolarButton}
})
export default class CreateInvoice extends Vue{
 invoiceStep: number = 1;
 invoice: IInvoice = {
   createdOn: new Date(),
   customerId: 0,
   lineItems: [],
   updatedOn: new Date()
 };
  customers: ICustomer[] = [];
  selectedCustomerId: number = 0;
  inventory:IProductInventory[] = [];
  lineItems: ILineItem[] = [];
  newItem: ILineItem = {product:undefined, quantity:0};

   addLineItem(){
     let newItem: ILineItem = {
       product: this.newItem.product,
       //quantity:parseInt(this.newItem.quantity.toString())
      quantity: Number(this.newItem.quantity.toString())
     };

     let existingItems = this.lineItems.map(item => item.product.id);

     if(existingItems.includes(newItem.product.id)){
       let lineItem = this.lineItems.find(
         item => item.product.id === newItem.product.id
       )
     }
   }





   get canGoNext(){
     if (this.invoiceStep == 1){
       return this.selectedCustomerId !== 0;
     }
     if(this.invoiceStep == 2){
       return true;
     }
     if(this.invoiceStep === 3){
       return false;
     }
    return false;
   }

  prev() : void {
    if (this.invoiceStep === 1){
      return;
    }
    this.invoiceStep -= 1;
  }

 next(): void {
   if(this.invoiceStep === 3){
     return;
   }
   this.invoiceStep += 1;
 }

  async initialize(): Promise<void> {
       this.customers = await customerService.getCustomers();
       this.inventory = await inventoryService.getInventory();
  }
  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
 .invoice-step {
 }


</style>
