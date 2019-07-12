import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pe0601ReceiptOmitComponent } from './pe0601-receipt-omit.component';

describe('Pe0601ReceiptOmitComponent', () => {
  let component: Pe0601ReceiptOmitComponent;
  let fixture: ComponentFixture<Pe0601ReceiptOmitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pe0601ReceiptOmitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pe0601ReceiptOmitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
