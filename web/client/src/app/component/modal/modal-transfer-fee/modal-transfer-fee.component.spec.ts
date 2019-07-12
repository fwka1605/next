import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalTransferFeeComponent } from './modal-transfer-fee.component';

describe('ModalTransferFeeComponent', () => {
  let component: ModalTransferFeeComponent;
  let fixture: ComponentFixture<ModalTransferFeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalTransferFeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalTransferFeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
