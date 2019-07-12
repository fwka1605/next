import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ph1301EbDataFormatListComponent } from './ph1301-eb-data-format-list.component';

describe('Ph1301EbDataFormatListComponent', () => {
  let component: Ph1301EbDataFormatListComponent;
  let fixture: ComponentFixture<Ph1301EbDataFormatListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ph1301EbDataFormatListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ph1301EbDataFormatListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
