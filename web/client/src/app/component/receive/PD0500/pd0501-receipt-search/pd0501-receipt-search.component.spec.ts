import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0501ReceiptSearchComponent } from './pd0501-receipt-search.component';

describe('Pd0501ReceiptSearchComponent', () => {
  let component: Pd0501ReceiptSearchComponent;
  let fixture: ComponentFixture<Pd0501ReceiptSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0501ReceiptSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0501ReceiptSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
