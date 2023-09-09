import numpy as np
import gymnasium as gym
import random
import pickle5 as pickle
import time

from gymnasium import Env
from threading import Thread
from gymnasium.spaces import Tuple, Discrete

class MathGame(Env):
    def __init__(self):
        self.n_factors = 13
        self.action_space = Tuple((Discrete(self.n_factors), Discrete(self.n_factors)))
        self.observation_space = Tuple((Discrete(self.n_factors), Discrete(self.n_factors)))
        self.state = self.observation_space.sample()
        
    def poseQuestion(self):
        return f"{self.state[0]} * {self.state[1]} ="
    
    def getAnswer(self, autoAnswer):
        if (autoAnswer):
            if (random.random() > 0.5):
                return self.state[0] * self.state[1]
            else:
                return -1
    
        maxAttempts = 3
        attempts = 0
        validAns = False
        while (validAns == False and attempts < maxAttempts):
            try:
                user_input = input(self.poseQuestion())
                answer = int(user_input)
                validAns = True
            except ValueError:
                print(f"You entered '{user_input}'.That is not a number.")
            attempts += 1
        return answer if validAns else -1
    
    def getIndex(self, factors):
        return factors[0]*self.n_factors + factors[1]
    
    def getAction(self, index):
        factor_1 = index//self.n_factors
        factor_2 = index % self.n_factors
        return np.array([factor_1, factor_2])
    
    def immediateReward(self, ans):
        print(f"State: {self.state}, Answer: {ans}")
        if (ans == (self.state[0] * self.state[1])):
            return 0
        return 1
    
    class Clock(Thread):
        def __init__(self):
            Thread.__init__(self)
            self.length = 60
            self.timeUp = False
        def run(self):
            time.sleep(self.length)
            self.timeUp = True
            
    def reset(self):
        self.state = self.observation_space.sample()
        self.clock = self.Clock()
        self.clock.start()
        return self.state, {}
        
    def step(self, action, auto=False):
        self.state = action
        answer = self.getAnswer(auto)
        
        reward = self.immediateReward(answer)
        
        done = self.clock.timeUp
        return self.state, reward, done, {}
    

## TRAINING
def makeQtable(action_space_size, observation_space_size):
    return np.zeros((observation_space_size, action_space_size))

def greedy_policy(Qtable, state_index):
    return np.argmax(Qtable[state_index][:])

def epsilon_greedy_policy(Qtable, state_index, epsilon,env):
    if (random.random() > epsilon):
        action_index = greedy_policy(Qtable, state_index)
        action = env.getAction(action_index)
    else:
        action = env.action_space.sample()
    return action

def train(n_episode, min_epsilon, max_epsilon, decay_rate, env, max_steps, Qtable):
    for episode in range(n_episode):
        state = env.reset()[0]
        terminated = False
        
        epsilon = min_epsilon + (max_epsilon - min_epsilon) * (np.exp(-decay_rate * episode))
        
        step = 0
        for step in range(max_steps):
            action = epsilon_greedy_policy(Qtable, env.getIndex(state), epsilon, env)
            next_state, reward, terminated, info = env.step(action, auto=True)
            Qtable[env.getIndex(state)][env.getIndex(action)] = Qtable[env.getIndex(state)][env.getIndex(action)] + learning_rate * (
                reward + gamma * np.max(Qtable[env.getIndex(next_state)]) - Qtable[env.getIndex(state)][env.getIndex(action)]
            )
            if terminated : break
            state = next_state
    return Qtable

class QModel():
    def __init__(self):
        self.qtable = qtab
        
    #returns the top n states
    def predict(self,state_n, n):
        return np.argsort(self.qtable[state_n][:])[-n:]

env = MathGame()
qtab = makeQtable(env.observation_space[0].n**2, env.action_space[0].n**2)

n_training_episodes = 1000
learning_rate = 0.7

max_steps = 60
gamma = 0.95

max_epsilon = 1.0
min_epsilon = 0.05
decay_rate = 0.0005

train(
    n_training_episodes,
    min_epsilon,
    max_epsilon,
    decay_rate,
    env,
    max_steps,
    qtab
)

model = QModel()
#serialize trained model
with open("q-learn-multiply-game.pkl", "wb") as f:
        pickle.dump(model, f)


