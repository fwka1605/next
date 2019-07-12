import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph0701GridSettingComponent } from './ph0701-grid-setting.component';

describe('Ph0701GridSettingComponent', () => {
  let component: Ph0701GridSettingComponent;
  let fixture: ComponentFixture<Ph0701GridSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph0701GridSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph0701GridSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
