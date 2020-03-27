// create a jumper based on the history object from react-router

export const createHistoryJumper = history => {
  function jumpTo(next) {
    if(!next || !history || typeof(history.push) !== 'function') return;
    history.push(next);
  }

  return {
    jumpTo
  }
}