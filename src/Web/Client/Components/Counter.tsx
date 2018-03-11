import * as React from 'react';
import { connect } from 'react-redux';
import { CounterState } from '../Types';
import { inc, dec } from '../Reducers/CounterReducer';


class Counter extends React.Component<any, CounterState> {
    render() {
        return (
            <div>
                <h3>{this.props.counter}</h3>
                <button onClick={this.props.onClickInc}>+</button>
                <button onClick={this.props.onClickDec}>-</button>
            </div>
        );
    }
}

const mapStateToProps = (x : any) => {
    return x;
};

const mapDispatchToProps = (dispatch: any) => {
    return {
        onClickInc: () => {
            dispatch(inc(1))
        },
        onClickDec: () => {
            dispatch(dec(1))
        }
    }
}

export const ConnectedCounter = connect(mapStateToProps, mapDispatchToProps)(Counter);
