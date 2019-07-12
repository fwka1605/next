import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph0801CollationSettingComponent } from './ph0801-collation-setting.component';

describe('Ph0801CollationSettingComponent', () => {
  let component: Ph0801CollationSettingComponent;
  let fixture: ComponentFixture<Ph0801CollationSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph0801CollationSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph0801CollationSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
