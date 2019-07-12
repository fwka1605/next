import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph1801MfAggrWebApiSettingComponent } from './ph1801-mf-aggr-web-api-setting.component';

describe('Ph1801MfAggrWebApiSettingComponent', () => {
  let component: Ph1801MfAggrWebApiSettingComponent;
  let fixture: ComponentFixture<Ph1801MfAggrWebApiSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph1801MfAggrWebApiSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph1801MfAggrWebApiSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
