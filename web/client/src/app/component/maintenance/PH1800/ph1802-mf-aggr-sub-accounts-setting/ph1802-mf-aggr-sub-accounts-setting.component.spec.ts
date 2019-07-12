import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph1802MfAggrSubAccountsSettingComponent } from './ph1802-mf-aggr-sub-accounts-setting.component';

describe('Ph1802MfAggrSubAccountsSettingComponent', () => {
  let component: Ph1802MfAggrSubAccountsSettingComponent;
  let fixture: ComponentFixture<Ph1802MfAggrSubAccountsSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph1802MfAggrSubAccountsSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph1802MfAggrSubAccountsSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
