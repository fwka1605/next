import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pc0701AccountTransferCreateComponent } from './pc0701-account-transfer-create.component';

describe('Pc0701AccountTransferCreateComponent', () => {
  let component: Pc0701AccountTransferCreateComponent;
  let fixture: ComponentFixture<Pc0701AccountTransferCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pc0701AccountTransferCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0701AccountTransferCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
