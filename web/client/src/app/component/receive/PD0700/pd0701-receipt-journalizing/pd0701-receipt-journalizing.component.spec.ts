import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0701ReceiptJournalizingComponent } from './pd0701-receipt-journalizing.component';

describe('Pd0701ReceiptJournalizingComponent', () => {
  let component: Pd0701ReceiptJournalizingComponent;
  let fixture: ComponentFixture<Pd0701ReceiptJournalizingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0701ReceiptJournalizingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0701ReceiptJournalizingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
