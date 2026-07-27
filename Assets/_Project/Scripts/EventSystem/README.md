# Event System + Reflex Setup

## 1. Create ReflexSettings
- Create `Assets/Resources` folder if missing
- Right-click Resources → Create → Reflex → Settings

## 2. Create RootScope
- Right-click any folder → Create → Reflex → RootScope
- Select the RootScope prefab
- Add `EventSystemInstaller` component to it

## 3. Link RootScope to ReflexSettings
- Select ReflexSettings (in Resources)
- Add the RootScope prefab to the RootScopes list

## 4. Use in scenes
- Add GameObject → Reflex → SceneScope to each scene needing DI
- Add EventPublisherDemo (or inject IEventBus) to test
