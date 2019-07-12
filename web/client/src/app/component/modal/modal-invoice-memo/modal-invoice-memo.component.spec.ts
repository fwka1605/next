import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalInvoiceMemoComponent } from './modal-invoice-memo.component';

describe('ModalInvoiceDemoComponent', () => {
  let component: ModalInvoiceMemoComponent;
  let fixture: ComponentFixture<ModalInvoiceMemoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ModalInvoiceMemoComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalInvoiceMemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
