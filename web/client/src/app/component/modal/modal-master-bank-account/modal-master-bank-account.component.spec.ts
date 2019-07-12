import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMasterBankAccountComponent } from './modal-master-bank-account.component';

describe('ModalMasterBankAccountComponent', () => {
  let component: ModalMasterBankAccountComponent;
  let fixture: ComponentFixture<ModalMasterBankAccountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalMasterBankAccountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMasterBankAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
