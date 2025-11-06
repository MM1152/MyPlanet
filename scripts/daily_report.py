import os
import datetime
from github import Github
from notion_client import Client

# ⚠️ 여기를 본인 정보로 수정하세요
NOTION_DB_ID = "2a31ff65-7f58-80ef-9cd8-cac9ad2a7c19"
TEAM_NAME = "4팀팀"
TARGET_REPO = "MyPlanet"  # 대상 레포지토리
REPO_OWNER = "MM1152"  # 레포지토리 소유자

def create_notion_heading(text, level=1):
    """Notion 헤딩 블록 생성"""
    return {
        "object": "block",
        "type": f"heading_{level}",
        f"heading_{level}": {
            "rich_text": [
                {
                    "type": "text",
                    "text": {"content": text}
                }
            ]
        }
    }

def create_notion_paragraph(text):
    """Notion 문단 블록 생성"""
    return {
        "object": "block",
        "type": "paragraph",
        "paragraph": {
            "rich_text": [
                {
                    "type": "text",
                    "text": {"content": text}
                }
            ]
        }
    }

def create_notion_bullet_list(items):
    """Notion 불릿 리스트 생성"""
    blocks = []
    for item in items:
        if isinstance(item, dict):
            # 이슈 정보가 포함된 경우
            author_info = f" (by @{item['author']})" if item.get('author') else ""
            text = f"{item['title']} (#{item['number']}){author_info}"
            if item.get('url'):
                blocks.append({
                    "object": "block",
                    "type": "bulleted_list_item",
                    "bulleted_list_item": {
                        "rich_text": [
                            {
                                "type": "text",
                                "text": {"content": text},
                                "href": item['url']
                            }
                        ]
                    }
                })
            else:
                blocks.append({
                    "object": "block",
                    "type": "bulleted_list_item",
                    "bulleted_list_item": {
                        "rich_text": [
                            {
                                "type": "text",
                                "text": {"content": text}
                            }
                        ]
                    }
                })
        else:
            # 일반 텍스트인 경우
            blocks.append({
                "object": "block",
                "type": "bulleted_list_item",
                "bulleted_list_item": {
                    "rich_text": [
                        {
                            "type": "text",
                            "text": {"content": str(item)}
                        }
                    ]
                }
            })
    return blocks

def create_divider():
    """Notion 구분선 생성"""
    return {
        "object": "block",
        "type": "divider",
        "divider": {}
    }

def get_repository_contributors(github, repo_owner, repo_name):
    """레포지토리 기여자 목록 가져오기"""
    try:
        repo = github.get_repo(f"{repo_owner}/{repo_name}")
        contributors = repo.get_contributors()
        contributor_list = [contributor.login for contributor in contributors]
        print(f"👥 발견된 기여자: {len(contributor_list)}명 - {', '.join(contributor_list[:5])}{'...' if len(contributor_list) > 5 else ''}")
        return contributor_list
    except Exception as e:
        print(f"⚠️ 기여자 목록 가져오기 실패: {e}")
        return []

def get_yesterday_completed_issues(github, repo_owner, repo_name, yesterday_str):
    """전날 완료된 모든 이슈들을 가져오기"""
    print(f"📋 {yesterday_str} 완료된 이슈를 검색 중...")
    
    # 전날에 닫힌 모든 이슈들 검색 (작성자 제한 없음)
    query = f"repo:{repo_owner}/{repo_name} is:issue closed:{yesterday_str}"
    issues = github.search_issues(query=query)
    
    completed_issues = []
    for issue in issues:
        completed_issues.append({
            'title': issue.title,
            'number': issue.number,
            'url': issue.html_url,
            'author': issue.user.login,
            'closed_at': issue.closed_at,
            'assignees': [assignee.login for assignee in issue.assignees] if issue.assignees else []
        })
    
    return completed_issues

def get_today_open_issues(github, repo_owner, repo_name):
    """오늘 진행할 모든 열린 이슈들을 가져오기"""
    print("📋 진행 중인 이슈를 검색 중...")
    
    # 모든 열린 이슈들 검색
    query = f"repo:{repo_owner}/{repo_name} is:issue is:open"
    issues = github.search_issues(query=query)
    
    open_issues = []
    for issue in issues:
        open_issues.append({
            'title': issue.title,
            'number': issue.number,
            'url': issue.html_url,
            'author': issue.user.login,
            'created_at': issue.created_at,
            'assignees': [assignee.login for assignee in issue.assignees] if issue.assignees else [],
            'labels': [label.name for label in issue.labels]
        })
    
    return open_issues

def get_open_pull_requests(github, repo_owner, repo_name):
    """현재 열린 Pull Request들 가져오기"""
    print("📋 진행 중인 PR을 검색 중...")
    
    try:
        # 현재 열린 PR들
        open_query = f"repo:{repo_owner}/{repo_name} is:pr is:open"
        open_prs = github.search_issues(query=open_query)
        
        open_pr_list = []
        for pr in open_prs:
            open_pr_list.append({
                'title': pr.title,
                'number': pr.number,
                'url': pr.html_url,
                'author': pr.user.login,
                'created_at': pr.created_at
            })
        
        return open_pr_list
    
    except Exception as e:
        print(f"⚠️ PR 검색 중 오류: {e}")
        return []

def main():
    # 현재 날짜 (한국 시간 기준)
    kst = datetime.timezone(datetime.timedelta(hours=9))
    now = datetime.datetime.now(kst)
    today = now.strftime("%Y-%m-%d")
    yesterday = (now - datetime.timedelta(days=1)).strftime("%Y-%m-%d")
    
    print(f"📅 {today} 일간보고를 생성합니다...")
    print(f"📅 대상 레포지토리: {REPO_OWNER}/{TARGET_REPO}")
    
    # GitHub API 초기화
    github = Github(os.environ["GITHUB_TOKEN"])
    
    try:
        # 레포지토리 기여자 목록 가져오기
        contributors = get_repository_contributors(github, REPO_OWNER, TARGET_REPO)
        
        # 어제 완료된 이슈들 (모든 사용자)
        completed_issues = get_yesterday_completed_issues(github, REPO_OWNER, TARGET_REPO, yesterday)
        print(f"✅ {yesterday} 완료된 이슈: {len(completed_issues)}개")
        
        # 오늘 진행할 이슈들 (모든 사용자)
        open_issues = get_today_open_issues(github, REPO_OWNER, TARGET_REPO)
        print(f"🔄 진행 중인 이슈: {len(open_issues)}개")
        
        # 진행 중인 PR
        open_prs = get_open_pull_requests(github, REPO_OWNER, TARGET_REPO)
        print(f"🔀 진행 중인 PR: {len(open_prs)}개")
        
    except Exception as e:
        print(f"⚠️ 데이터 수집 중 오류: {e}")
        contributors = []
        completed_issues = []
        open_issues = []
        open_prs = []
    
    print("📝 보고서 내용 생성 완료")
    
    # Notion에 저장
    print("📤 Notion에 저장하는 중...")
    
    notion = Client(auth=os.environ["NOTION_TOKEN"])
    
    # Notion 블록 구성
    blocks = []
    
    # 제목
    blocks.append(create_notion_heading(f"{today} 일간보고: {TEAM_NAME}", 1))
    
    # 프로젝트 정보
    blocks.append(create_notion_paragraph(f"📂 대상 레포지토리: {REPO_OWNER}/{TARGET_REPO}"))
    blocks.append(create_notion_paragraph(f"👥 활성 기여자: {len(contributors)}명"))
    blocks.append(create_divider())
    
    # 전일 보고
    blocks.append(create_notion_heading("전일 보고", 2))
    
    # 완료된 이슈
    blocks.append(create_notion_heading("완료", 3))
    if completed_issues:
        blocks.extend(create_notion_bullet_list(completed_issues))
    else:
        blocks.extend(create_notion_bullet_list(["완료된 이슈 없음"]))
    
    # 미완료 작업 (진행 중인 이슈들)
    blocks.append(create_notion_heading("미완료 (사유, 처리)", 3))
    if open_issues:
        # 진행 중인 이슈들
        priority_issues = []
        for issue in open_issues[:10]:  # 최대 10개
            status = "진행중"
            if issue['assignees']:
                status = f"진행중 (담당: {', '.join(issue['assignees'])})"
            item = issue.copy()
            item['title'] = f"{issue['title']} - {status}"
            priority_issues.append(item)
        blocks.extend(create_notion_bullet_list(priority_issues))
    else:
        blocks.extend(create_notion_bullet_list(["미완료 이슈 없음"]))
    
    blocks.append(create_divider())
    
    # 금일 보고
    blocks.append(create_notion_heading("금일 보고", 2))
    
    # 오늘 계획된 작업들
    today_tasks = []
    
    # 진행 중인 이슈들 기반 작업 계획
    if open_issues:
        high_priority = [issue for issue in open_issues if 'high' in str(issue['labels']).lower() or 'urgent' in str(issue['labels']).lower()]
        if high_priority:
            today_tasks.append("🔥 긴급/높은 우선순위 이슈 처리")
            for issue in high_priority[:3]:
                today_tasks.append(f"  • {issue['title']} (#{issue['number']})")
        
        # 일반 진행 중인 작업
        for issue in open_issues[:5]:
            assignee_info = f" (담당: {issue['assignees'][0]})" if issue['assignees'] else ""
            today_tasks.append(f"• {issue['title']} (#{issue['number']}){assignee_info}")
    
    # 진행 중인 PR 리뷰
    if open_prs:
        today_tasks.append("📝 PR 리뷰 및 병합 작업")
        for pr in open_prs[:3]:
            today_tasks.append(f"  • {pr['title']} (#{pr['number']}) by @{pr['author']}")
    
    # 할일이 없으면 기본 메시지
    if not today_tasks:
        today_tasks.append("새로운 이슈 및 개발 작업 계획 수립")
    
    blocks.extend(create_notion_bullet_list(today_tasks))
    
    try:
        new_page = notion.pages.create(
            parent={"database_id": NOTION_DB_ID},
            properties={
                "제목": {
                    "title": [
                        {
                            "text": {
                                "content": f"{today} 일간보고"
                            }
                        }
                    ]
                },
                "작성일": {
                    "date": {
                        "start": today
                    }
                }
            },
            children=blocks
        )
        
        print("✅ 일간보고가 성공적으로 Notion에 저장되었습니다!")
        print(f"📄 페이지 ID: {new_page['id']}")
        
    except Exception as e:
        print(f"❌ Notion 저장 중 오류 발생: {e}")
        raise

if __name__ == "__main__":
    main()
