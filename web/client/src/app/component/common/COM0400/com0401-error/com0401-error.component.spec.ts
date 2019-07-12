import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0401ErrorComponent } from './com0401-error.component';

describe('Com0401ErrorComponent', () => {
  let component: Com0401ErrorComponent;
  let fixture: ComponentFixture<Com0401ErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0401ErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0401ErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
