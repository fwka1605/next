import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0601TopComponent } from './com0601-top.component';

describe('Com0601TopComponent', () => {
  let component: Com0601TopComponent;
  let fixture: ComponentFixture<Com0601TopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0601TopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0601TopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
