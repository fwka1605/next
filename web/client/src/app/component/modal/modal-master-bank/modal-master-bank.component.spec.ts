import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMasterBankComponent } from './modal-master-bank.component';

describe('ModalMasterBankComponent', () => {
  let component: ModalMasterBankComponent;
  let fixture: ComponentFixture<ModalMasterBankComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalMasterBankComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMasterBankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
