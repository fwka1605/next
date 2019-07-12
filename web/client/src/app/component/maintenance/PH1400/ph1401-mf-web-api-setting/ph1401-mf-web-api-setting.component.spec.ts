import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph1401MfWebApiSettingComponent } from './ph1401-mf-web-api-setting.component';

describe('Ph1401MfWebApiSettingComponent', () => {
  let component: Ph1401MfWebApiSettingComponent;
  let fixture: ComponentFixture<Ph1401MfWebApiSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph1401MfWebApiSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph1401MfWebApiSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
