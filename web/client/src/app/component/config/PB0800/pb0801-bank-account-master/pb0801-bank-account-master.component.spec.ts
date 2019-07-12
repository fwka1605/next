import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0801BankAccountMasterComponent } from './pb0801-bank-account-master.component';

describe('Pb0801BankAccountMasterComponent', () => {
  let component: Pb0801BankAccountMasterComponent;
  let fixture: ComponentFixture<Pb0801BankAccountMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0801BankAccountMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0801BankAccountMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
