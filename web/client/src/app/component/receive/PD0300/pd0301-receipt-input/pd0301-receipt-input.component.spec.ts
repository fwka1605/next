import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0301ReceiptInputComponent } from './pd0301-receipt-input.component';

describe('Pd0301ReceiptInputComponent', () => {
  let component: Pd0301ReceiptInputComponent;
  let fixture: ComponentFixture<Pd0301ReceiptInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0301ReceiptInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0301ReceiptInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
