import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0000MainComponent } from './com0000-main.component';

describe('Com0000MainComponent', () => {
  let component: Com0000MainComponent;
  let fixture: ComponentFixture<Com0000MainComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0000MainComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0000MainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
