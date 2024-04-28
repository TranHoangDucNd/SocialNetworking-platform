import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DatingProfileComponent } from './dating-profile.component';

describe('DatingProfileComponent', () => {
  let component: DatingProfileComponent;
  let fixture: ComponentFixture<DatingProfileComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DatingProfileComponent]
    });
    fixture = TestBed.createComponent(DatingProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
