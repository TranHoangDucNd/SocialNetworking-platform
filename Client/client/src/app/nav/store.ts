import {BehaviorSubject, Observable} from 'rxjs';
import {map} from 'rxjs/operators';

export interface Action<T> {
  type: string;
  payload?: T;
}

export type Reducer<T, V> = (state: T, action: V) => T;

export class Store<T> {
  private state$: BehaviorSubject<T>;

  constructor(private reducer: Reducer<T, Action<any>>, initialState: T) {
    this.state$ = new BehaviorSubject<T>(initialState);
  }

  // Get the current state.
  get value(): T {
    return this.state$.getValue();
  }

  // Observe a piece of state.
  select<K extends keyof T>(name: K): Observable<T[K]> {
    return this.state$.asObservable().pipe(map(state => state[name]));
  }

  // Dispatch an action.
  dispatch(action: Action<any>): void {
    try {
      const newState = this.reducer(this.value, action);
      this.state$.next(newState);
      console.log('New state:', newState);
    } catch (error) {
      console.error('Error occurred while dispatching action:', error);
    }
  }
}
