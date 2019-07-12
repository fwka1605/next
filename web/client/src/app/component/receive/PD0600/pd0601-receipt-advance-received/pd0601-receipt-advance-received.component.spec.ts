import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0601ReceiptAdvanceReceivedComponent } from './pd0601-receipt-advance-received.component';

describe('Pd0601ReceiptAdvanceReceivedComponent', () => {
  let component: Pd0601ReceiptAdvanceReceivedComponent;
  let fixture: ComponentFixture<Pd0601ReceiptAdvanceReceivedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0601ReceiptAdvanceReceivedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0601ReceiptAdvanceReceivedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
