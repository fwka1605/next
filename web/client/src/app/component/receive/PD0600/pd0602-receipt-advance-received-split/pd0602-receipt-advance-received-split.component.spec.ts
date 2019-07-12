import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0602ReceiptAdvanceReceivedSplitComponent } from './pd0602-receipt-advance-received-split.component';

describe('Pd0602ReceiptAdvanceReceivedSplitComponent', () => {
  let component: Pd0602ReceiptAdvanceReceivedSplitComponent;
  let fixture: ComponentFixture<Pd0602ReceiptAdvanceReceivedSplitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0602ReceiptAdvanceReceivedSplitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0602ReceiptAdvanceReceivedSplitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
