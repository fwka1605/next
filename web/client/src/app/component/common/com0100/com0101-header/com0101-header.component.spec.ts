import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0101HeaderComponent } from './com0101-header.component';

describe('Com0101HeaderComponent', () => {
  let component: Com0101HeaderComponent;
  let fixture: ComponentFixture<Com0101HeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0101HeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0101HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
