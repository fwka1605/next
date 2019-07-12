import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Com0201FooterComponent } from './com0201-footer.component';

describe('Com0201FooterComponent', () => {
  let component: Com0201FooterComponent;
  let fixture: ComponentFixture<Com0201FooterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Com0201FooterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Com0201FooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
