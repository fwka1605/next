import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pj0101UserSettingComponent } from './pj0101-user-setting.component';

describe('Pj0101UserSettingComponent', () => {
  let component: Pj0101UserSettingComponent;
  let fixture: ComponentFixture<Pj0101UserSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pj0101UserSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pj0101UserSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
