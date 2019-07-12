import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pa0101LoginComponent } from './pa0101-login.component';

describe('Pa0101LoginComponent', () => {
  let component: Pa0101LoginComponent;
  let fixture: ComponentFixture<Pa0101LoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pa0101LoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pa0101LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
