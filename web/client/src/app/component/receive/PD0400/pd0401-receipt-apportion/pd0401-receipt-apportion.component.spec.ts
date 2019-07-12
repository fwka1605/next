import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0401ReceiptApportionComponent } from './pd0401-receipt-apportion.component';

describe('Pd0401ReceiptApportionComponent', () => {
  let component: Pd0401ReceiptApportionComponent;
  let fixture: ComponentFixture<Pd0401ReceiptApportionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0401ReceiptApportionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0401ReceiptApportionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
