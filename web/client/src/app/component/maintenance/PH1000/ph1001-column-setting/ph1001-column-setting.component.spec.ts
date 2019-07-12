import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph1001ColumnSettingComponent } from './ph1001-column-setting.component';

describe('Ph1001ColumnSettingComponent', () => {
  let component: Ph1001ColumnSettingComponent;
  let fixture: ComponentFixture<Ph1001ColumnSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph1001ColumnSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph1001ColumnSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
