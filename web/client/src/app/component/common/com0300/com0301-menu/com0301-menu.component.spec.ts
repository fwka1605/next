import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0301MenuComponent } from './com0301-menu.component';

describe('Com0301MenuComponent', () => {
  let component: Com0301MenuComponent;
  let fixture: ComponentFixture<Com0301MenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0301MenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0301MenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
