import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0801AccountTransferImportComponent } from './pc0801-account-transfer-import.component';

describe('Pc0801AccountTransferImportComponent', () => {
  let component: Pc0801AccountTransferImportComponent;
  let fixture: ComponentFixture<Pc0801AccountTransferImportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0801AccountTransferImportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0801AccountTransferImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
