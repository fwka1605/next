import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0801ReceiptSectionTransferComponent } from './pd0801-receipt-section-transfer.component';

describe('Pd0801ReceiptSectionTransferComponent', () => {
  let component: Pd0801ReceiptSectionTransferComponent;
  let fixture: ComponentFixture<Pd0801ReceiptSectionTransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0801ReceiptSectionTransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0801ReceiptSectionTransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
