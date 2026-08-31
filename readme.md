# DearFriend

![cover](./presskit/cover.png)

## Project Status

This repository contains the current development of **DearFriend**.

The current vertical slice is located at:

`./unity/DearFriend`

It represents roughly the **first part of the story** and is currently our main technical base.

We are now in the final phase of production. The team is continuing to work on the story, dialogue, assets, animation and audio.

---

## Getting Started

### Unity

**Unity version:** `6000.3.15f1 LTS`
**Target:** iPad, 4:3

To run the current vertical slice:

1. Open `./unity/DearFriend` as a Unity project.
2. Press **Play** in Unity.

### Vertical Slice Reference Video

If the project cannot be run, or if you need a reference for the intended behavior, a recorded playthrough of the vertical slice is available here:

**[Watch the vertical slice](https://www.youtube.com/watch?v=ya9sq1jCNZ4)**

The video can also be used as a reference when comparing behavior during integration and bug fixing.


## Repository Structure

The Unity folder contains the main project as well as a number of smaller experimental projects.

```text
/
├── README.md
├── ...
└── unity/
|   ├── DearFriend/          ← Current vertical slice / main project
|   ├── ...-lab/             ← Experiments and isolated prototypes
|   └── ...
└── ...
```

### Labs

Projects suffixed with `-lab` are **experiments, isolated prototypes or archives of previous tests**.

They are not part of the current vertical slice and do not need to be reviewed unless specifically mentioned.

Some new features may first be developed in an isolated lab before being integrated into `DearFriend`.

---

# Current Production Status

| Area              | Status         | Notes                                                 |
| ----------------- | -------------- | ----------------------------------------------------- |
| Vertical slice    | 🟢 Ready       | Current technical base in `./unity/DearFriend`        |
| Story structure   | 🟡 In progress | Priority: lock the story structure                    |
| Dialogue          | 🟡 In progress | Still being written and refined                       |
| Yarn              | 🟡 In progress | Updated separately to reflect the new story structure |
| Messaging app     | 🟡 In progress | First developed in isolation, then integrated         |
| New visual assets | 🔴 To do       | New 2D and 3D assets                                  |
| Animation         | 🟡 In progress | AI mocap and integration with the final story         |
| Audio             | 🟡 In progress | Composer's work + Sylvia's audio need integration     |
| Bug fixing        | 🟡 In progress | Ongoing                                               |

---

# Current Priorities

## 1. Story

**Highest priority: lock the story structure.**

We currently have the rough structure of the story, but some aspects still need to be clarified:

* When specific events happen.
* Whether some events happen at all.
* The exact progression of certain scenes.
* In particular, the **Day 2 discussion about the late friend** still needs to be written/resolved.

Once these structural decisions are made, the story should be considered **locked**.

The dialogue itself can continue to be refined afterward. This includes:

* Trimming dialogue.
* Improving formulations.
* Making conversations feel more natural.
* Making dialogue less informative/expository.

The final game should be written in **French**.

---

## 2. Yarn / Dialogue Structure

Story development is currently happening in a **separate Yarn-only project**, without Unity or code integration.

The Yarn files currently being written are significantly different from those in the vertical slice because the structure of the story has changed.

This includes differences in:

* Filenames.
* Story organization.
* Dialogue structure.

The new Yarn version should therefore **not currently be considered a drop-in replacement** for the Yarn files in the vertical slice.

Once the story structure is locked, the updated dialogue structure will need to be integrated into the main Unity project.

---

## 3. Messaging App

A new **2D messaging application** needs to be created.

The first version will be developed as an isolated prototype/lab.

Once the interaction and visuals are established, it will need to be integrated into the main `DearFriend` Unity project.

---

## 4. New Visual Assets

The following assets still need to be produced.

### 2D

* Messaging app.
* 3–4 pictures for the contest.
* 3–4 files visible on the desktop, including invoices.

### 3D

* Cooking timer.
* Tupperware.
* Hat with cat ears.

---

# Animation

Animation work is still ongoing and partly depends on the final story structure.

### Current Work

* Produce character movements using AI-based mocap.
* Once the story is locked, place the animations at the appropriate moments in the story.
* Change Sylvia's walking pace — currently too slow.
* Investigate alternatives to the current NavMesh-based approach for scripted character movement.
* Improve how Sylvia sits on the sofa.
* Add head movement where appropriate.
* Handle states such as whether the coffee is empty or not.

### Optional

Sylvia could have a different outfit on Day 3.

Possible changes:

* Different shirt/color.
* Cat-ear hat made for Carnaval.

---

# Audio

Audio still needs an integration pass.

### To Do

* Integrate the composer's work.
* Clean up and integrate Sylvia's audio.
* Decide whether to test a voice for Camille.

### Dialogue Audio

When the player advances to the next dialogue line, the audio associated with the previous line should stop immediately.

---

# Dialogue Interaction

The dialogue interaction still needs some adjustments.

### Advancing Dialogue

The player should be able to **tap anywhere on the screen** to advance the dialogue.

Some playtesters did not understand that they needed to tap to continue.

We need to improve the visual indication that the dialogue can be advanced.

Possible directions:

* Increase the size of the downward arrow.
* Animate the arrow slightly.
* Find another subtle visual indication.

The exact solution is still open.

---

# Onboarding

The beginning of the experience still needs some adjustments :

Animate the password entry at the beginning:

`1... 2... 3... 4`

The final implementation depends on the decision regarding the password.

The password box should also contain an instruction in French:

> **Toucher l'écran pour rentrer le mot de passe**

---

# Day Transitions

The pacing of transitions between days needs refinement.

In particular:

* Timing of the transition.
* Timing of the associated animations.
* What happens visually during the transition.
* Coordination between the transition and other actions.

---

# Camera / Zoom Out

The zoom-out system is already integrated into the vertical slice.

Previous zoom experiments are archived in the labs and do not currently need to be reviewed.

The current integration, however, still has some issues.

### Known Issues

The camera does not always zoom out smoothly.

This may be caused by timing conflicts between:

* Camera movement.
* Character movement.
* Other actions.
* Animations.

There is also an issue where tapping the screen during or around a camera zoom can sometimes cause the animation state to change unexpectedly.

This currently feels buggy and needs investigation.

---

# Interactions

Some interactions need a general refinement pass.

In particular:

* Closing windows and similar UI interactions.
* Touch behavior on iPad.
* Preventing interactions from unintentionally affecting animation or camera states.

---

# Ending / Offboarding

The end of the experience still needs to be defined both creatively and technically.

Questions include:

* Do we show credits?
* How does the experience return to the beginning?
* What is the cleanest technical way to reset/restart the game?

---

# Inactivity

We need to decide what happens when nobody interacts with the experience for a while.

One possibility:

1. After a period of inactivity, display a dialogue.
2. Ask whether the player wants to continue.
3. Alternatively, offer the option to restart from the beginning.

The exact behavior and timing still need to be defined.

---

# Testing

Development would benefit from a way to start the game from a **specific point in the story**.

For example:

* Start directly on a specific day.
* Start at a particular dialogue or sequence.
* Test a specific interaction without replaying everything before it.

This should make testing and integration significantly faster during the final production phase.

---

# Project Cleanup

The current Unity project is not necessarily organized as cleanly as we would ideally like.

However, because we are late in production, a large-scale cleanup or restructuring may introduce unnecessary risk.

The priority is therefore **stability and successful integration rather than achieving a perfectly organized project**.

Small, safe improvements are welcome where they make integration or debugging easier.

---

# Integration & Bug Fixing

We are continuing to develop content while integration work happens in parallel.

The integration work should primarily focus on:

* Fixing bugs.
* Making small technical adjustments.
* Integrating incoming content and updates.
* Maintaining a stable project while the team continues production.
* Ensuring interactions work appropriately on iPad.
* Helping bring the different parts of the project together cleanly.
* Preparing a stable and polished final iPad version.

The project is already late in production, so the intention is **not to rewrite major systems unnecessarily**.

When possible, prefer targeted and reliable fixes over large architectural changes.

---

# Open Technical Questions

Some technical decisions may need to be discussed as development continues:

* Best way to integrate incoming team changes while working from separate forks/branches.
* How much project cleanup is useful versus risky at this stage.
* How to handle scripted character movement and the current NavMesh approach.
* How to implement the end / credits / restart flow cleanly.
* How to handle player inactivity.
* How to quickly jump to specific days or moments for testing.
* How to keep the project stable while story, animation, assets and audio continue to change.
* Final iPad integration and polish.

---

# General Principle

At this stage of production, the priorities are:

**Lock the story → finish the content → integrate → fix → test → polish.**

The vertical slice gives us the existing technical foundation. The objective is to build on that foundation and bring the project to a stable final iPad experience without introducing unnecessary complexity.