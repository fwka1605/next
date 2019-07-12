import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1801BankBranchMasterComponent } from './pb1801-bank-branch-master.component';

describe('Pb1801BankBranchMasterComponent', () => {
  let component: Pb1801BankBranchMasterComponent;
  let fixture: ComponentFixture<Pb1801BankBranchMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1801BankBranchMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1801BankBranchMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
