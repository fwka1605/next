import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pa0201LogoutComponent } from './pa0201-logout.component';

describe('Pa0201LogoutComponent', () => {
  let component: Pa0201LogoutComponent;
  let fixture: ComponentFixture<Pa0201LogoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pa0201LogoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pa0201LogoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
