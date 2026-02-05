namespace Warehouse.Domain.Enums;

public enum DocumentType
{
    Receipt = 1,   // Incoming from Supplier
    Shipment = 2,  // Outgoing to Customer
    WriteOff = 3   // Loss/Damage (inventory correction)
}
